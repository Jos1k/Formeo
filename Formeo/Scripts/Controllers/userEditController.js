var userEditController = function ($scope, $modalInstance, $window, $http, UploadPrinObject, user, companies) {

    $scope.companies = companies;

    $scope.user = {
        Id: user.Id,
        UserName: user.UserName,
        Company: $.grep($scope.companies, function (e) { return e.id == user.Company.Id; })[0],
        Email: user.Email,
        Address: user.Address,
        Postal: user.Postal,
        City: user.City,
        Country: user.Country,
        SelectedRole: user.SelectedRole,
        NewPassword:''
    };

    $scope.setDefaultCompany = function () {
        var defaultcompanyId = null;
        if ($scope.user.SelectedRole == 'Customer') {
            defaultcompanyId = $.grep($scope.companies, function (e) { return e.isCustomer == true; })[0];
        }
        if ($scope.user.SelectedRole == 'Producer') {
            defaultcompanyId = $.grep($scope.companies, function (e) { return e.isCustomer == false; })[0];
        }
        $scope.user.Company = defaultcompanyId;
    };
    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.EditUser = function () {
        if ($scope.user.Company.id) {
            $scope.user.Company = $scope.user.Company.id;
        }
        $http({
            method: 'POST',
            url: '/Home/EditUser',
            headers: { 'Content-Type': 'application/json;' },
            data: {
                'Id': $scope.user.Id,
                'Email': $scope.user.Email,
                'City': $scope.user.City,
                'Country': $scope.user.Country,
                'Postal': $scope.user.Postal,
                'Address': $scope.user.Address,
                'CompanyId':$scope.user.Company,
                'SelectedRole': $scope.user.SelectedRole,
                'NewPassword': $scope.user.NewPassword
            }
        }).
           then(function (response) {
               $modalInstance.close(JSON.parse(response.data));
           }, function (response) {
               $window.alert('error updating company');
               $modalInstance.dismiss('cancel');
           });
    };

}