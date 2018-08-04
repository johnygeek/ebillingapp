function HomeViewModel(app, dataModel) {
    var alertTemplate = '<div class="alert alert-danger alert-dismissible">'
        + '<strong>Sorry!</strong> No customer found.'
        + '<button type="button" class="close" data-dismiss="alert" aria-label="Close">'
        + '<span aria-hidden="true">&times;</span>'
        + '</button></div>';
    var self = this;

    self.firstName = ko.observable();
    self.lastName = ko.observable();
    self.mobileNumber = ko.observable();
    self.email = ko.observable();
    self.customerId = ko.observable('');


    self.save = function () {
        $('.modal').show();
        var customer = {
            firstName: self.firstName(),
            lastName: self.lastName(),
            email: self.email(),
            mobile: self.mobileNumber()
        };
        $.ajax({
            method: 'post',
            url: app.dataModel.customerAPIUrl,
            data: JSON.stringify(customer),
            contentType: 'application/json;charset=utf-8',
            success: function (data) {
                self.customerId(data.custId);
                if (self.customerId !== '') {
                    processOrder();
                }
            }
        });
    }

    self.cancel = function () {
        clearFields();
    }

    self.getCustomer = function (e) {
        var key = !self.email() ? self.mobileNumber() : self.email();
        $.get(app.dataModel.customerAPIUrl + '?key=' + key, function (data) {
            if (data !== undefined && data !== null) {
                if (data.custId !== '') {
                    self.firstName(data.firstName);
                    self.lastName(data.lastName);
                    if (!self.email())
                        self.email(data.email);
                    if (!self.mobileNumber())
                        self.mobileNumber(data.mobile);
                    self.customerId(data.custId);
                }
                else {
                    $(e.target).before(alertTemplate);
                }
            }
        });
    }

    processOrder = function () {
        var order = { customerId: self.customerId() };
        var items = [];
        $('input:checked').each(function () {
            var item = new Object();
            item.id = $(this).prop('id').split('_')[0];
            item.type = $(this).prop('id').split('_')[1];
            item.quantity = $(this).parent().find('input[type="number"]').val();
            items.push(item);
        });
        $.ajax({
            method: 'post',
            url: app.dataModel.orderAPIUrl,
            data: { orderInfo: order, items: items },
            dataType: 'json',
            success: function (data) {
                if (data.status === 'success') {
                    $('.modal').hide();
                    alert('Thank you for making an order. your order details are send via email and sms');
                }
            }
        });
    }

    clearFields = function () {
        self.firstName('');
        self.lastName('');
        self.mobileNumber('');
        self.email('');
        self.customerId('');
        $('input:checked').parent().find('input[type="number"]').val('');
        $('input:checked').prop('checked', false);
    }

    return self;
}

app.addViewModel({
    name: "Home",
    bindingMemberName: "home",
    factory: HomeViewModel
});
