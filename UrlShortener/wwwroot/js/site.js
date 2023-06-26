
function myFunction() {
    var copyText = document.getElementById("getLink").textContent;
    navigator.clipboard.writeText(copyText);
    document.getElementById("copyButton").classList.remove("btn-warning");
    document.getElementById("copyButton").classList.add("btn-success");
    document.getElementById("copyButton").innerHTML="Copied! <i class='bi bi-scissors'></i>";
}