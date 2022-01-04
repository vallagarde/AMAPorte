
function validate() {
    var valid = false;

    if (document.getElementById("contrat").checked) {
        valid = true;
    }

    if (valid) {
        alert("Validation Succesfull");
    }
    else {
        alert("Veuillez accepter les conditions pour continuer");
        return false;
    }

}