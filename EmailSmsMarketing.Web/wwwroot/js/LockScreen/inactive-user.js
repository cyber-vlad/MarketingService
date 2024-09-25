(function () {
    var lastActiveTime = new Date();

    document.addEventListener('mousemove', function () {
        lastActiveTime = new Date();
    });

    document.addEventListener('keydown', function () {
        lastActiveTime = new Date();
    });

    setInterval(function () {
        var inactivityTime = (new Date() - lastActiveTime) / 1000; // Time in seconds
        if (inactivityTime > 1200) { // Check after 20 minutes (1200 seconds)
            window.location.href = "/Account/LockScreen"; // Redirect to LockScreen action
        }
    }, 1000); // Check every second
})();