$(document).ready(function () {
    // Event handler for the Easy button
    $('#easyBtn').click(function () {
        startGame('1');
    });

    // Event handler for the Medium button
    $('#mediumBtn').click(function () {
        startGame('2');
    });

    // Event handler for the Hard button
    $('#hardBtn').click(function () {
        startGame('3');
    });

    //event handlers
    var homeButton = document.getElementById('homeButton');
    if (homeButton) {
        homeButton.addEventListener('click', function () {
            window.location.href = 'Index';
        });
    }

    var scoreboardButton = document.getElementById('scoreboardButton');
    if (scoreboardButton) {
        scoreboardButton.addEventListener('click', function () {
            window.location.href = '/Score/Index';
        });
    }

    var logOff = document.getElementById('logOff');
    if (logOff) {
        logOff.addEventListener('click', function () {
            window.location.href = '/Home/LogOff';
        });
    }
});

function startGame(gameMode) {
    //local storage save
    localStorage.setItem('gameMode', gameMode);
    localStorage.setItem('tryCount', 0);
    localStorage.setItem('score', 0);

    $.ajax({
        url: '/Game/Start',
        type: 'GET',
        data: { mode: gameMode },
        success: function (response) {
            // Start the game timer with a duration
            startGameTimer(response.AnswerTimeInMinits);

            drawQuestionAndAnswers(response);
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
            checkError();
        }
    });
}

function drawQuestionAndAnswers(response) {

    $('#gameMode-div').hide();
    $('#gameArea-div').show();
    // Create an <img> element and set its source to the image data
    var imgElement = $('<img>').attr('src', response.Question);
    // Append the <img> element to the 'imageContainer' div
    $('#imageContainer').empty().append(imgElement);
    $('#buttonContainer').empty();
    var btn = response.Solutions;
    btn.forEach(function (button) {
        // Create a new button element
        var newButton = $('<button class="btn-lg" onclick = CheckAnswer(' + button.IsCorrectAnswer + ')>' + button.Answer + '</button>');
        $('#buttonContainer').append(newButton);
    });
}

function CheckAnswer(isCorrect) {
    if (isCorrect) {
        var audioUrl = 'https://commondatastorage.googleapis.com/codeskulptor-assets/sounddogs/explosion.mp3';
        var audio = new Audio(audioUrl);

        // Play the audio
        audio.play().then(() => {
            Toastify({
                text: "Answer Correct!!!",
                style: {
                    background: "green",
                },
                duration: 3000

            }).showToast();

            audio.onended = function () {
                var scoreAsString = localStorage.getItem('score');

                // Convert the score string to a number
                var score = parseInt(scoreAsString, 10);

                // Check if the conversion was successful (not NaN)
                if (!isNaN(score)) {
                    // Add 10 to the score
                    score += 10;
                }

                localStorage.setItem('score', score);
                localStorage.setItem('tryCount', 0);

                var scoreDisplayElement = document.getElementById('scoreDisplay');
                if (scoreDisplayElement) {
                    scoreDisplayElement.textContent = 'Score: ' + score;
                }

                var gameMode = localStorage.getItem('gameMode');
                $.ajax({
                    url: '/Game/Start',
                    type: 'GET',
                    data: { mode: gameMode },
                    success: function (response) {
                        drawQuestionAndAnswers(response);
                    },
                    error: function (xhr, status, error) {
                        console.error(xhr.responseText);
                        checkError();
                    }
                });
            };
        }).catch(e => {
            console.log("Error playing the audio: ", e);
            window.location.href = '/Game/Index';
        });

    }
    else
    {
        var tryCount = localStorage.getItem('tryCount');
        tryCount++
        localStorage.setItem('tryCount', tryCount);

        if (tryCount >= 3) {
            saveHighScore();
            playAudio();
        }
        else {
            Toastify({
                text: "Please Try Again.",
                style: {
                    background: "red",
                },
                duration: 3000

            }).showToast();
        }

    }
}

function saveHighScore() {
    var gameMode = localStorage.getItem('gameMode');
    var score = localStorage.getItem('score');

    $.ajax({
        url: '/Game/SaveHighScore',
        type: 'PoST',
        data: { mode: gameMode, scoreValue: score },
        success: function (response) {
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
}

function startGameTimer(durationInMinutes) {
    var durationInMillis = durationInMinutes * 60 * 1000; // Convert minutes to milliseconds
    var endTime = Date.now() + durationInMillis; // Calculate the end time

    var timerElement = document.getElementById('timer');

    var timerInterval = setInterval(function () {
        var remainingTime = endTime - Date.now(); // Calculate the remaining time
        if (remainingTime <= 0) {
            clearInterval(timerInterval); // Stop the timer when time is up
            gameOver();
            timerElement.textContent = 'Game Over!';
        } else {
            var minutes = Math.floor((remainingTime / (1000 * 60)) % 60);
            var seconds = Math.floor((remainingTime / 1000) % 60);
            timerElement.textContent = 'Time Remaining: ' + minutes + 'm ' + seconds + 's';
        }
    }, 1000); // Update every second
}

function gameOver() {
    saveHighScore();
    playAudio();
}

function playAudio() {
    var audioUrl = 'https://commondatastorage.googleapis.com/codeskulptor-assets/sounddogs/explosion.mp3';
    var audio = new Audio(audioUrl);

    // Play the audio
    audio.play().then(() => {
        Toastify({
            text: "Game Over.",
            style: {
                background: "red",
            },
            duration: 3000

        }).showToast();

        audio.onended = function () {
            // Redirect after the audio has finished playing
            window.location.href = '/Game/Index';
        };
    }).catch(e => {
        console.log("Error playing the audio: ", e);
        window.location.href = '/Game/Index';
    });
}

function checkError() {
    Toastify({
        text: "Game Loading Error. Please try again in while.",
        style: {
            background: "red",
        },
        duration: 3000

    }).showToast();
}