var companyEditController = function ($scope, $modalInstance, $window, $http, UploadPrinObject, company) {

    $scope.company = {
        id: company.id,
        orgNumber: company.orgNumber,
        country: company.country,
        taxNumber: company.taxNumber,
        companyName: company.companyName,
        isCustomer: company.isCustomer
    };

    $scope.price = 0.01;
    $scope.printObjectInfoModal = "";

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
    
    $scope.EditCompany = function () {
        $http({
            method: 'POST',
            url: '/Home/EditCompany',
            headers: { 'Content-Type': 'application/json;' },
            data: {
                'Name': $scope.company.companyName,
                'ID': $scope.company.id,
                'Country': $scope.company.country,
                'OrgNumber': $scope.company.orgNumber,
                'TaxNumber': $scope.company.taxNumber,
                'IsCustomer': $scope.company.isCustomer
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