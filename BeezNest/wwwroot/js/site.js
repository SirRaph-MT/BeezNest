// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



function registration() {
    debugger
    var data = {};

    data.FirstName = $("#firstName").val();
    data.LastName = $("#lastName").val();
    data.PhoneNumber = $("#phoneNumber").val();
    data.Address = $("#address").val();
    data.Email = $("#email").val();
    data.State = $("#state").val();
    data.City = $("#city").val();
    data.Password = $("#password").val();
    data.role = $("#role").val();
    data.ConfirmPassword = $("#confirmPassword").val();

    if (!data.FirstName) {
        errorAlert("First name is required");
        return;
    }
    if (!data.State) {
        errorAlert("Please Select State");
        return;
    }
    if (!data.City) {
        errorAlert("Select your city");
        return;
    }
    if (!data.LastName) {
        errorAlert("Last name is required");
        return;
    }
    if (!data.PhoneNumber) {
        errorAlert("Phone number is required");
        return;
    } 
    if (!data.Address) {
        errorAlert("Address is required");
        return;
    }
 
    if (!data.Email) {
        errorAlert("Email is required");
        return;
    } else {
     
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(data.Email)) {
            errorAlert("Invalid email format. Please enter a valid email.");
            return;
        }

      
        const validExtensions = ["com", "org", "net"]; 
        const emailExtension = data.Email.split('.').pop(); 
       
        if (!validExtensions.includes(emailExtension)) {
            errorAlert(`Invalid email format. Allowed formats are: ${validExtensions.join(', ')}`);
            return;
        }
    }


    if (!data.Password) {
        errorAlert("Password is required");
        return;
    }
    if (data.Password.length < 5) {
        errorAlert("Password must be greater than 5 digits");
        return;
    }
    if (data.Password !== data.ConfirmPassword) {
        errorAlert("Confirm password does not match password");
        return;
    }
    $.ajax({
        type: 'POST',
        url: '/Account/Register',
        dataType: 'json',
        data:
        {
            userDetails: JSON.stringify(data)
        },
        success: function (result) {
            if (!result.isError) {
                debugger
                var url = "/Account/Login";
                //window.location.href = "/Account/Login";
                successAlertWithRedirect(result.msg, url);
            } else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            $('#validationSummary').text("Error occurred. Please try again.");
        }
    });
}


function showSidebar() {
    document.getElementById("toggleablesidebar").classList.toggle("hidden");
}

// Add to Cart Function
function addToCart() {
    debugger
    const storedCartItems = JSON.parse(localStorage.getItem('cart')) || [];

    const productId = $("#ProductId").val();
    const numberOfItem = $("#numberOfItem").val();
    const price = $("#price").text().replace("Price: ₦", "").trim();
    const quantity = parseInt($("#quantityInput").val(), 10);
    const selectedColor = $("#colorSelect").val() || "Default Color";
    const userId = $("#Email").text() || "Guest";
    const productModel = $("#prodModel").text().replace("Product: ", "").trim();
    const specifications = $("#spec").text().replace("Specifications: ", "").trim();
    const imageUrl = $("#mainImage").attr("src");

    const cartItem = {
        UploadProductId: productId,
        Name: productModel,
        Specifications: specifications,
        Price: price,
        Quantity: quantity,
        Color: selectedColor,
        ImageUrl: imageUrl,
        UserId: userId,
    };

    // Check if item already exists
    const existingIndex = storedCartItems.findIndex(
        item => item.UploadProductId === cartItem.UploadProductId && item.Color === cartItem.Color
    );

    if (existingIndex !== -1) {
        storedCartItems[existingIndex].Quantity += cartItem.Quantity;
    } else {
        storedCartItems.push(cartItem);
    }

    localStorage.setItem("cart", JSON.stringify(storedCartItems));
    successAlert("Item successfully added to cart")
    // Update badge count
    updateBadgeCount();
}

// Update Cart Badge Count
function updateBadgeCount() {
    const cart = JSON.parse(localStorage.getItem('cart')) || [];
    const totalItems = cart.reduce((sum, item) => sum + item.Quantity, 0);

    const badge = document.querySelector('.cartCounterBadge');
    if (totalItems === 0) {
        badge.style.display = 'none';
    } else {
        badge.textContent = totalItems;
        badge.style.display = 'inline-block';
    }
}

// Run Badge Update on Page Load
window.onload = updateBadgeCount;

// Display Cart Items on Icon Click
document.querySelector('.iconShopping').addEventListener('click', function () {
    const cartItems = JSON.parse(localStorage.getItem('cart')) || [];
    const cartTableBody = document.getElementById('cartTableBody');
    const checkoutContainer = document.getElementById('checkoutContainer');

    cartTableBody.innerHTML = '';

    if (cartItems.length === 0) {
        cartTableBody.innerHTML = `
            <tr>
                <td colspan="7" class="text-center">Your cart is empty</td>
            </tr>`;
        if (checkoutContainer) checkoutContainer.innerHTML = '';
        return;
    }

    cartItems.forEach(item => {
        const price = parseFloat(item.Price.toString().replace(/,/g, "")) || 0;
        const quantity = parseInt(item.Quantity) || 0;
        const total = price * quantity;

        const row = `
        <tr>
            <td><img style="height:40px;" src="${item.ImageUrl}" alt="product image" /></td>
            <td>${item.Name}</td>
            <td>${item.Specifications || 'N/A'}</td>
            <td>₦${price.toLocaleString()}</td>
            <td>
                <div class="d-flex align-items-center">
                    <button type="button" class="btn btn-outline-secondary decrease-quantity" data-id="${item.UploadProductId}">-</button>
                    <input type="text" id="quantity-${item.UploadProductId}" value="${quantity}" class="form-control text-center mx-2" style="width: 60px;" readonly />
                    <button type="button" class="btn btn-outline-secondary increase-quantity" data-id="${item.UploadProductId}">+</button>
                </div>
            </td>
            <td id="total-${item.UploadProductId}">₦${total.toLocaleString()}</td>
            <td><button type="button" class="btn btn-danger remove-item" data-id="${item.UploadProductId}">Remove</button></td>
        </tr>`;
        cartTableBody.innerHTML += row;
    });

    if (checkoutContainer && cartItems.length > 0) {
        checkoutContainer.innerHTML = `
            <button type="button" style="width:75%;" class="btn text-white" onclick="checkUserAndProceedToCheckout()">Checkout</button>`;
    } else if (checkoutContainer) {
        checkoutContainer.innerHTML = '';
    }
});

// Check User and Proceed to Checkout
function checkUserAndProceedToCheckout() {
    const userId = $("#UserEmail").text().trim();

    if (!userId) {
        errorAlert("You need to register or log in before proceeding to checkout.");
        window.location.href = "/Account/registration";
    } else {
        const checkoutModal = new bootstrap.Modal(document.getElementById('checkoutModal'));
        checkoutModal.show();
    }
}

document.querySelector('.iconShopping').addEventListener('click', function () {
    const checkoutModal = document.querySelector('#checkoutModal');
    if (checkoutModal) {
        checkoutModal.addEventListener('shown.bs.modal', function () {
            const cartItems = JSON.parse(localStorage.getItem('cart')) || [];
            const checkoutTableBody = document.getElementById('checkoutTableBody');
            const haulageFeeField = document.getElementById('haulageFee');
            const grandTotalField = document.getElementById('grandTotal');

            checkoutTableBody.innerHTML = ''; 

            let totalItems = 0;
            let totalPrice = 0;

            cartItems.forEach(item => {
                const quantity = parseInt(item.Quantity) || 0;
                const price = parseFloat(item.Price.toString().replace(/,/g, "")) || 0;
                const total = price * quantity;

                totalItems += quantity;
                totalPrice += total;

                const row = `
                    <tr>
                        <td>${item.Name}</td>
                        <td>${item.Specifications || 'N/A'}</td>
                        <td>₦${price.toLocaleString()}</td>
                        <td>${quantity}</td>
                        <td>₦${total.toLocaleString()}</td>
                    </tr>
                `;
                checkoutTableBody.innerHTML += row;
            });

            // Calculate haulage fee and grand total
            const haulageFee = parseFloat((totalPrice * 0.02).toFixed(2));
            const grandTotal = totalPrice + haulageFee;

            // Update haulage fee and grand total fields
            if (haulageFeeField) {
                haulageFeeField.value = `₦${haulageFee.toLocaleString()}`;
            }
            if (grandTotalField) {
                grandTotalField.value = `₦${grandTotal.toLocaleString()}`;
            }
        });
    }
});

//function checkOutNow(userEmail) {
//    debugger
//    var data = {};
//    const cartItems = JSON.parse(localStorage.getItem('cart')) || [];
//    let totalItems = 0;
//    let totalPrice = 0;
//    let itemsList = [];

//    cartItems.forEach(item => {
//        var quantity = parseInt(item.Quantity) || 0;
//        var productName = item.Name;
//        var specifications = item.Specifications;
//        var unitPrice = parseFloat(item.Price.toString().replace(/,/g, "")) || 0;
//        var total = unitPrice * quantity;

//        totalItems += quantity;
//        totalPrice += total;

//        itemsList.push({
//            Name: productName,
//            Quantity: quantity,
//            Specifications: specifications,
//            UnitPrice: unitPrice,
//            Total: total
//        });
//    });

//    const haulageFee = parseFloat((totalPrice * 0.02).toFixed(2));
//    data.GrandTotal = totalPrice + haulageFee;
//    data.ClientEmail = userEmail;
//    data.StocksInString = JSON.stringify(itemsList);
//    $.ajax({
//        type: 'POST',
//        url: '/Payment/UserCheckOut',
//        dataType: 'json',
//        data:
//        {
//            stockDetails: JSON.stringify(data)
//        },
//        success: function (result) {
//            if (!result.isError) {
//                localStorage.removeItem('cart')
//                var url = "/Home/Index";
//                successAlertWithRedirect(result.msg, url);
//            } else {
//                errorAlert(result.msg)
//            }
//        },
//        error: function (ex) {
//            $('#validationSummary').text("Error occurred. Please try again.");
//        }
//    });
//}

//function checkOutNow(userEmail) {
//    debugger;

//    // Retrieve cart items from localStorage
//    const cartItems = JSON.parse(localStorage.getItem('cart')) || [];
//    if (cartItems.length === 0) {
//        errorAlert("Your cart is empty. Add items to proceed.");
//        return;
//    }

//    let totalItems = 0;
//    let totalPrice = 0;
//    const itemsList = cartItems.map(item => {
//        const quantity = parseInt(item.Quantity) || 0;
//        const unitPrice = parseFloat(item.Price.toString().replace(/,/g, "")) || 0;
//        const total = unitPrice * quantity;

//        totalItems += quantity;
//        totalPrice += total;

//        return {
//            Name: item.Name,
//            Quantity: quantity,
//            Specifications: item.Specifications,
//            UnitPrice: unitPrice,
//            Total: total
//        };
//    });


//    const haulageFee = parseFloat((totalPrice * 0.02).toFixed(2));
//    const grandTotal = totalPrice + haulageFee;


//    const formData = new FormData();
//    formData.append("stockDetails", JSON.stringify({
//        GrandTotal: grandTotal,
//        ClientEmail: userEmail,
//        StocksInString: JSON.stringify(itemsList)
//    }));


//    const fileInput = document.querySelector("input[type='file']");
//    if (fileInput && fileInput.files.length > 0) {
//        formData.append("ProofOfPayment", fileInput.files[0]);
//    } else {
//        errorAlert("Please upload proof of payment before submitting.");
//        return;
//    }


//    $.ajax({
//        type: 'POST',
//        url: '/Payment/UserCheckOut',
//        data: formData,
//        processData: false,
//        contentType: false,
//        success: function (result) {
//            if (!result.isError) {
//                localStorage.removeItem('cart');
//                successAlertWithRedirect(result.msg, "/Payment/PaymentHistory");
//            } else {
//                errorAlert(result.msg);
//            }
//        },
//        error: function () {
//            $('#validationSummary').text("Error occurred. Please try again.");
//        }
//    });
//}






// Event Delegation for Quantity Update and Item Removal

function checkOutNow(userEmail) {
    debugger;

    // Retrieve cart items from localStorage
    const cartItems = JSON.parse(localStorage.getItem('cart')) || [];
    if (cartItems.length === 0) {
        errorAlert("Your cart is empty. Add items to proceed.");
        return;
    }

    let totalItems = 0;
    let totalPrice = 0;
    const itemsList = cartItems.map(item => {
        const quantity = parseInt(item.Quantity) || 0;
        const unitPrice = parseFloat(item.Price.toString().replace(/,/g, "")) || 0;
        const total = unitPrice * quantity;

        totalItems += quantity;
        totalPrice += total;

        return {
            Name: item.Name,
            Quantity: quantity,
            Specifications: item.Specifications,
            UnitPrice: unitPrice,
            Total: total
        };
    });

    const haulageFee = parseFloat((totalPrice * 0.02).toFixed(2));
    const grandTotal = totalPrice + haulageFee;

    const formData = new FormData();
    formData.append("stockDetails", JSON.stringify({
        GrandTotal: grandTotal,
        ClientEmail: userEmail,
        StocksInString: JSON.stringify(itemsList)
    }));

    const fileInput = document.querySelector("input[type='file']");
    if (fileInput && fileInput.files.length > 0) {
        formData.append("ProofOfPayment", fileInput.files[0]);
    } else {
        errorAlert("Please upload proof of payment before submitting.");
        return;
    }

    $.ajax({
        type: 'POST',
        url: '/Payment/UserCheckOut',
        data: formData,
        processData: false,
        contentType: false,
        success: function (result) {
            if (!result.isError) {
                // Clear the cart from localStorage
                localStorage.removeItem('cart');

                // Notification update: Update order count on success
                if (result.orderCount !== undefined) {
                    debugger
                    const orderCountBadge = document.getElementById('orderCount');
                    if (orderCountBadge) {
                        orderCountBadge.textContent = result.orderCount;
                    }
                }

                // Redirect or show success message
                successAlertWithRedirect(result.msg, "/Payment/PaymentHistory");
            } else {
                errorAlert(result.msg);
            }
        },
        error: function () {
            $('#validationSummary').text("Error occurred. Please try again.");
        }
    });
}


document.getElementById('cartTableBody').addEventListener('click', function (e) {
    const cartItems = JSON.parse(localStorage.getItem('cart')) || [];
    const target = e.target;

    if (target.classList.contains('decrease-quantity')) {
        const productId = target.dataset.id;
        const index = cartItems.findIndex(item => item.UploadProductId === productId);

        if (index !== -1 && cartItems[index].Quantity > 1) {
            cartItems[index].Quantity -= 1;
            localStorage.setItem('cart', JSON.stringify(cartItems));
            document.getElementById(`quantity-${productId}`).value = cartItems[index].Quantity;
            updateBadgeCount();
        }
    } else if (target.classList.contains('increase-quantity')) {
        const productId = target.dataset.id;
        const index = cartItems.findIndex(item => item.UploadProductId === productId);

        if (index !== -1) {
            cartItems[index].Quantity += 1;
            localStorage.setItem('cart', JSON.stringify(cartItems));
            document.getElementById(`quantity-${productId}`).value = cartItems[index].Quantity;
            updateBadgeCount();
        }
    } else if (target.classList.contains('remove-item')) {
        const productId = target.dataset.id;
        const updatedCartItems = cartItems.filter(item => item.UploadProductId !== productId);
        localStorage.setItem('cart', JSON.stringify(updatedCartItems));
        updateBadgeCount();
        target.closest('tr').remove();

        if (updatedCartItems.length === 0) {
            document.getElementById('cartTableBody').innerHTML = `
                <tr>
                    <td colspan="7" class="text-center">Your cart is empty</td>
                </tr>`;
        }
    }
});

