
    $(document).ready(function () {
        $('#scoreTbl').DataTable({

        });
    var homeButton = document.getElementById('homeButton');
    if (homeButton) {
        homeButton.addEventListener('click', function () {
            window.location.href = '/Game/Index';
        });
        }
    });
