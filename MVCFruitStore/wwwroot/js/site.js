// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$("#login").click(function () {
    var name = $("#name").val();
    var psw = $("#password").val();
    if (name.length === 0 || psw.length() === 0) {
        $("#err_msg").html = "All fields are required";
        return;
    }
});


var q = document.getElementsByClassName("Quantity");
for (var i = 0; i < q.length; i++) {
    var input = q[i];
    input.addEventListener('change', function (event) {
        var input = event.target;
        console.log(input);
        if (isNaN(input.value) || input.value < 0) {
            input.value = 1;
        }
        getTotalPrice();
    });
    function getTotalPrice() {
        var total = 0;
        var cartContainer = document.getElementsByClassName("cart_prod_price");
        var changeInt = parseInt(cartContainer.length);
        for (var i = 0; i < changeInt; i++) {
            var cartContainers = cartContainer[i];
            var priceElement = cartContainers.getElementsByClassName("pPrice")[0];
            var quantityElement = cartContainers.getElementsByClassName("Quantity")[0];
            var price = parseFloat(priceElement.value);
            var quantity = quantityElement.value;
            total = total + (price * quantity);

        }
        document.getElementById("totalPrice").innerHTML = "$" + total;
    }
}
function downloadPruchase() {
    alert("Your download is starting!!")
}
