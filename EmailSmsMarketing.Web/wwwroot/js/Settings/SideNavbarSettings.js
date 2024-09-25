function openOption(evt, optionName) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(optionName).style.display
        = "block";
    evt.currentTarget.className += " active";

    localStorage.setItem('activeTab', optionName);
}

window.onload = function () {
    var activeTab = localStorage.getItem('activeTab');
    if (activeTab) {
        openOption(null, activeTab);
    } else {
        document.getElementById("defaultOpen").click();
        
    }
};