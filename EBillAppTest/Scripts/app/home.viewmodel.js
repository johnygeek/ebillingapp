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
                    self.processOrder();
                }
            }
        });
    }

    self.getCustomer = function (vm, e) {
        if (e.keyCode === 13) {
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
        else {
            return true;
        }
    }

    self.processOrder = function () {
        var order = { customerId: self.customerId() };
        var items = [];
        $('input:checked').each(function () {
            var item = new Object();
            item.id = $(this).prop('id').split('_')[0];
            item.type = $(this).prop('id').split('_')[1];
            items.push(item);
        });
        $.ajax({
            method: 'post',
            url: app.dataModel.orderAPIUrl,
            data: { orderInfo: order, items: items },
            dataType: 'json',
            success: function (data) {
                if (data.status === 'success') {
                    alert('Thank you for making an order. your order details are send via email and sms');
                }
            }
        });
    }

    return self;
}

app.addViewModel({
    name: "Home",
    bindingMemberName: "home",
    factory: HomeViewModel
});
