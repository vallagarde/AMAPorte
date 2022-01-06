// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $.ajax({
        url: "/Panier/PanierIsEmpty",
        type: 'GET',
        success: function (res) {
            if (res == true) {
                $("#panierVide").show();
                $("#panierRemplit").hide();
            } else {
                $("#panierVide").hide();
                $("#panierRemplit").show();
                $(document).ready(function () {
                    $.ajax({
                        url: "/Boutique/QuantitePanier",
                        type: 'GET',
                        success: function (res) {

                            $("#nombreArticle").html(res);
                        }
                    });
                });
            }
        }
    });
});

$(document).ready(function () {
    $.ajax({
        url: "/Login/UtilisateurEstConnecte",
        type: 'GET',
        success: function (res) {            
            if (res == true) {
                $("#compteperso").show();
                $("#compteadmin").hide();
                $("#connexion").hide();
                $(document).ready(function () {
                    $.ajax({
                        url: "/Login/UtilisateurEstAdmin",
                        type: 'GET',
                        success: function (res) {
                            if (res == true) {
                                $("#compteperso").hide();
                                $("#connexion").hide();
                                $("#compteadmin").show();
                            }
                        }
                    });
                });
            }
            else {
                $("#compteperso").hide();
                $("#compteadmin").hide();
                $("#connexion").show();
            }
        }
    });
});


