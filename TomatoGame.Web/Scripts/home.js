$(document).ready(function () {

    //google auth
    function onSignIn(googleUser) {
        var profile = googleUser.getBasicProfile();
    }

    function signOut() {
        var auth2 = gapi.auth2.getAuthInstance();
        auth2.signOut().then(function () {
            console.log('User signed out.');
        });
    }

    // AJAX login form submission

    $('#loginForm').submit(function (event) {
        event.preventDefault(); // Prevent default form submission
        var formData = $(this).serialize(); // Serialize form data

        $.ajax({
            url: '/Home/LoginAsync',
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response) {
                    window.location.href = '/Game/Index';
                }
                else {
                    Toastify({
                        text: "The Username or Password does not match. Please try again.",
                        style: {
                            background: "red",
                        },
                        duration: 3000

                    }).showToast();
                }
            },
            error: function (xhr, status, error) {
                // Handle error response
                console.error('Login error:', error);
            }
        });
    });

    // AJAX signup form submission
    $('#signupForm').submit(function (event) {
        event.preventDefault(); // Prevent default form submission
        var passwordText = $('#Password').val();
        var confirmPassword = $('#ConfirmPassword').val();

        if (passwordText !== confirmPassword) {
            event.preventDefault(); // Stop form from submitting
            Toastify({
                text: "The Passwords does not match. Please try again.",
                style: {
                    background: "red",
                },
                duration: 3000

            }).showToast();
            return;
        }

        if (isExistsUser($('#Email').val())) {
            event.preventDefault(); // Stop form from submitting
            Toastify({
                text: "User Already Registered.",
                style: {
                    background: "red",
                },
                duration: 3000

            }).showToast();
            return;
        }

        var formData = $(this).serialize(); // Serialize form data

        $.ajax({
            url: '/Home/SignUpAsync',
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response == true) {
                    window.location.href = '/Game/Index';
                }
                else {
                    Toastify({
                        text: "Something is wrong. Please try again.",
                        style: {
                            background: "red",
                        },
                        duration: 3000

                    }).showToast();
                }
            },
            error: function (xhr, status, error) {
                // Handle error response
                console.error('Signup error:', error);
            }
        });
    });
});

function isExistsUser(userEmail) {
    var result = false
    $.ajax({
        url: '/Home/IsUserExists?email=' + userEmail,
        type: 'GET',
        async:false,
        success: function (response) {
            if (response) {
                result = true;
            }
        },
        error: function (xhr, status, error) {
            // Handle error response
            console.error('Signup error:', error);
        }
    });
    return result;
}
