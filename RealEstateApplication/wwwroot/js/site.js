
function deleteHome(id) {
    fetch(`/Homes/Delete/${id}`, {
        method: 'POST'
    })
        .then(response => {
            window.location.href = '/Homes/Index';
        })
}

document.addEventListener('DOMContentLoaded', function () {
    var statesDropdown = document.getElementById('statesDropdown');
    var citiesDropdown = document.getElementById('citiesDropdown');

    //commenting out the following code as the external api is not working
    //statesDropdown.addEventListener('change', function () {
    //    var selectedState = this.value;
    //    if (selectedState) {
    //        fetch(`/Homes/GetCities?state=${selectedState}`)
    //            .then(response => {
    //                if (!response.ok) {
    //                    throw new Error('Network response was not ok');
    //                }
    //                return response.json();
    //            })
    //            .then(data => {
    //                citiesDropdown.innerHTML = '<option value="">Select City</option>';
    //                data.forEach(city => {
    //                    var option = document.createElement('option');
    //                    option.textContent = city;
    //                    option.value = city;
    //                    citiesDropdown.appendChild(option);
    //                });
    //            })
    //            .catch(error => {
    //                console.error('There was a problem with the fetch operation:', error);
    //            });
    //    } else {
    //        citiesDropdown.innerHTML = '<option value="">No Cities Were Found</option>';
    //    } 

    statesDropdown.addEventListener('change', function () {
        var selectedState = this.value;

        if (selectedState) {
            citiesDropdown.innerHTML = '<option value="">Select City</option>'; // Reset dropdown to only have default option
            var option = document.createElement('option'); 
            option.textContent = 'City'; 
            option.value = 'City'; 
            citiesDropdown.appendChild(option); 
        } else {
            citiesDropdown.innerHTML = '<option value="">No Cities Were Found</option>';
        }
    });


});