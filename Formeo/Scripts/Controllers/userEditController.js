var userEditController = function ($scope, $modalInstance, $window, $http, UploadPrinObject, user) {

    $scope.user = {
        Id: user.Id,
        UserName: user.UserName,
        //Company: user.Company,
        Email: user.Email,
        Address: user.Address,
        Postal: user.Postal,
        City: user.City,
        Country: user.Country,
        SelectedRole: user.SelectedRole,
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
    
    $scope.EditUser = function () {
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
                //'SelectedRole': $scope.user.isCustomer,
                //'CompanyId': $scope.user.Company
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