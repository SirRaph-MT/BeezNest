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
    const sidebar = document.querySelector('.sidebar');
    sidebar.classList.toggle('show');
}



function addToCart(button) {
    debugger
    const storedCartItems = JSON.parse(localStorage.getItem("cart")) || [];

    let productCard, productId, maxStock, price, quantity, productModel, specifications, imageUrl, userId, selectedColor;

    if (button) {
        // Home page context
        productCard = button.closest(".col-12");
        productId = productCard.querySelector(".productId").value;
        maxStock = parseInt(productCard.querySelector(".numberOfItem").textContent.replace("Item left: ", "").trim()) || 0;
        price = productCard.querySelector(".price").textContent.replace("Price: ₦", "").trim();
        quantity = parseInt(productCard.querySelector(".quantityInput").value, 10) || 1;
        productModel = productCard.querySelector(".card-title").textContent.trim();
        specifications = productCard.querySelector(".specifications").textContent.replace("Specifications: ", "").trim();
        imageUrl = productCard.querySelector(".card-img-top").src || "default_image_url";
        selectedColor = "No Color Selected"; // Default color for homepage items
    } else {
        // Product details page context
        productId = document.querySelector("#productId").value;
        maxStock = parseInt(document.querySelector("#numberOfItem").textContent.replace("Available Items: ", "").trim()) || 0;
        price = document.querySelector("#price").textContent.replace("Price: ₦", "").trim();
        quantity = parseInt(document.querySelector("#quantityInput").value, 10) || 1;
        productModel = document.querySelector("#prodModel").textContent.replace("Product: ", "").trim();
        specifications = document.querySelector("#spec").textContent.replace("Specifications: ", "").trim();
        imageUrl = document.querySelector("#mainImage").src || "default_image_url";
        selectedColor = document.querySelector("#colorSelect")?.value || "No Color Selected"; // Allow color selection
    }

    userId = document.querySelector("#Email")?.textContent.trim() || "Guest";

    // Create a standardized cartItem
    const cartItem = {
        UploadProductId: productId,
        Name: productModel,
        Specifications: specifications,
        Price: price,
        Quantity: quantity,
        Color: selectedColor,
        ImageUrl: imageUrl,
        MaxStock: maxStock,
        UserId: userId,
    };

    // Check if the product with the same ID and color already exists in the cart
    const existingIndex = storedCartItems.findIndex(
        item => item.UploadProductId === cartItem.UploadProductId && item.Color === cartItem.Color
    );

    let currentTotalQuantity = 0;

    if (existingIndex !== -1) {
        // If the product exists, update the quantity
        currentTotalQuantity = storedCartItems[existingIndex].Quantity + quantity;

        if (currentTotalQuantity > maxStock) {
            errorAlert(`Only ${maxStock} items are available in stock. Current quantity in cart: ${storedCartItems[existingIndex].Quantity}`);
            return;
        }

        storedCartItems[existingIndex].Quantity = currentTotalQuantity;
    } else {
        // If the product does not exist, check stock and add it
        if (quantity > maxStock) {
            errorAlert(`Only ${maxStock} items are available in stock.`);
            return;
        }
        storedCartItems.push(cartItem);
    }

    // Save the updated cart back to localStorage
    localStorage.setItem("cart", JSON.stringify(storedCartItems));
    successAlert("Item successfully added to cart");

    // Update the cart badge
    updateBadgeCount();
}





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


window.onload = updateBadgeCount;



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
        const maxStock = parseInt(item.MaxStock) || 0; 
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
                    <button type="button" class="btn btn-outline-secondary increase-quantity" data-id="${item.UploadProductId}" data-max="${maxStock}">+</button>
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

document.getElementById('cartTableBody').addEventListener('click', function (e) {
    debugger;
    const cartItems = JSON.parse(localStorage.getItem('cart')) || [];
    const target = e.target;

    if (target.classList.contains('decrease-quantity')) {
        const productId = target.dataset.id;
        const index = cartItems.findIndex(item => item.UploadProductId === productId);

        if (index !== -1 && cartItems[index].Quantity > 1) {
            cartItems[index].Quantity -= 1;

            // Update localStorage
            localStorage.setItem('cart', JSON.stringify(cartItems));

            // Update DOM for quantity and total price
            document.getElementById(`quantity-${productId}`).value = cartItems[index].Quantity;
            updateItemTotal(productId, cartItems[index].Price, cartItems[index].Quantity);

            // Update badge count
            updateBadgeCount();
        }
    } else if (target.classList.contains('increase-quantity')) {
        const productId = target.dataset.id;
        const maxStock = parseInt(target.dataset.max) || 0;
        const index = cartItems.findIndex(item => item.UploadProductId === productId);

        if (index !== -1) {
            if (cartItems[index].Quantity < maxStock) {
                cartItems[index].Quantity += 1;

                // Update localStorage
                localStorage.setItem('cart', JSON.stringify(cartItems));

                // Update DOM for quantity and total price
                document.getElementById(`quantity-${productId}`).value = cartItems[index].Quantity;
                updateItemTotal(productId, cartItems[index].Price, cartItems[index].Quantity);

                // Update badge count
                updateBadgeCount();
            } else {
                errorAlert(`Only ${maxStock} items are available in stock.`);
            }
        }
    } else if (target.classList.contains('remove-item')) {
        const productId = target.dataset.id;
        const updatedCartItems = cartItems.filter(item => item.UploadProductId !== productId);

        // Update localStorage and remove the row
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



function updateItemTotal(productId, price, quantity) {
    const totalPrice = parseFloat(price.toString().replace(/,/g, "")) * quantity;
    const totalElement = document.getElementById(`total-${productId}`);

    if (totalElement) {
        totalElement.textContent = `₦${totalPrice.toLocaleString()}`;
    }
}



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


            const haulageFee = parseFloat((totalPrice * 0.02).toFixed(2));
            const grandTotal = totalPrice + haulageFee;


            if (haulageFeeField) {
                haulageFeeField.value = `₦${haulageFee.toLocaleString()} (2% of your total orders)`;
            }
            if (grandTotalField) {
                grandTotalField.value = `₦${grandTotal.toLocaleString()}`;
            }
        });
    }
});


function checkOutNow(userEmail) {
    debugger;


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

                localStorage.removeItem('cart');


                if (result.orderCount !== undefined) {
                    debugger
                    const orderCountBadge = document.getElementById('orderCount');
                    if (orderCountBadge) {
                        orderCountBadge.textContent = result.orderCount++;
                    }
                }
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




function viewProduct(paymentId, isFromAdmin) {
    $.ajax({
        type: 'GET',
        url: '/Payment/GetOrderDetails',
        dataType: "json",
        data: {
            paymentId: paymentId,
            isFromAdmin: isFromAdmin
        },
        success: function (result) {
            if (!result.isError) {
                var modalOrderTableBody = $("#modalOrderTableBody");
                modalOrderTableBody.html("");
                var stocks = result.stock;

                if (stocks && stocks.length > 0) {
                    stocks.forEach(s => {
                        const row = `<tr>
                            <td>${s.name}</td>
                            <td>${s.specifications}</td>
                            <td>${s.quantity}</td>
                            <td>₦${s.unitPrice}</td>
                            <td>₦${s.total}</td>
                        </tr>`;
                        modalOrderTableBody.append(row);
                    });
                } else {
                    modalOrderTableBody.append(`<tr><td colspan="5">No items found.</td></tr>`);
                }
                if (isFromAdmin) {
                    $('#orderModal').modal('show');
                }
                else {
                    $('#orderModal').modal('show');
                }
            } else {
                errorAlert(result.msg);
            }
        },
        error: function () {
            $('#validationSummary').text("Error occurred. Please try again.");
        }
    });
}



function updateStars(averageRating) {
    const ratingStarsContainer = document.getElementById("ratingStars");

   
    let starsHtml = "";
    for (let i = 1; i <= 5; i++) {
        if (i <= Math.floor(averageRating)) {
            starsHtml += '<i class="fas fa-star text-warning"></i>'; 
        } else if (i - averageRating < 1) {
            starsHtml += '<i class="fas fa-star-half-alt text-warning"></i>'; 
        } else {
            starsHtml += '<i class="far fa-star text-warning"></i>'; 
        }
    }

   
    ratingStarsContainer.innerHTML = starsHtml;
}

function setModalRating(rating) {
    const stars = document.querySelectorAll("#modalRatingStars i");
    stars.forEach((star, index) => {
        star.className = index < rating ? "fas fa-star" : "far fa-star";
    });
    document.getElementById("modalRatingValue").value = rating; 
}

// Submit the rating and review
function submitModalRating() {
    const productId = document.getElementById("productId").value;
    const rating = document.getElementById("modalRatingValue").value;
    const review = document.getElementById("reviewText").value;

    if (!rating || rating < 1 || rating > 5) {
        errorAlert("Please select a valid rating before submitting.");
        return;
    }

    // Prepare the data to send
    const data = {
        productId: productId,
        rating: parseInt(rating),
        review: review
    };

    // Send the rating to the server
    fetch('/Rating/SubmitRating', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
        .then(response => {
            if (response.ok) {
                return response.json();
            } else {
                throw new Error('Failed to submit rating');
            }
        })
        .then(result => {
            successAlert('Thank you for your feedback!');
            // Optionally update the average rating and reviews dynamically
            updateStars(result.newAverageRating);
            document.getElementById("totalRatings").innerText = `Rated by ${result.totalRatings} users`;
            document.getElementById("reviewText").value = ""; // Clear the review text
            document.querySelectorAll("#modalRatingStars i").forEach(star => star.className = "far fa-star"); // Reset stars
            document.getElementById("modalRatingValue").value = "0"; // Reset rating
            // Close the modal
            const modal = bootstrap.Modal.getInstance(document.getElementById("ratingModal"));
            modal.hide();
        })
        .catch(error => {
            errorAlert('Failed.You can only rate products you have purchased');
            console.error('Error:', error);
        });
}
