var adminPageController = function ($scope, $window, $modal, $http) {
    $scope.userModel = {
        username: '',
        password: '',
        email: '',
        address: '',
        postal: '',
        country: '',
        companyId: '',
        selectedRole: 'Admin'
    };

    $scope.companyModel = {
        id: '',
        orgNumber: '',
        country: '',
        taxNumber: '',
        companyName: '',
        isCustomer: false
    };

    $scope.EMPTY = "";

    $scope.selectedMainMenu = '/Clients';
    $scope.selectedClientsMenu = '/AddUser';
    $scope.selectedCompaniesMenu = '/AddCompany';
    $scope.helloVariable = 'I work!';
    $scope.mainMenu = [];
    $scope.menuType = "0";
    $scope.selectedUser = $scope.EMPTY;
    $scope.selectedCompany = $scope.EMPTY;

    $scope.templateUserEdit = '';
    $scope.templateCompanyEdit = '';

    $scope.loadTemplatesForModals = function () {
        $http({
            method: 'GET',
            url: '/Home/EditCompanyModal'
        }).
        then(function (response) {
            $scope.templateCompanyEdit = response.data;
        });
        $http({
            method: 'GET',
            url: '/Home/EditUserModal'
        }).
        then(function (response) {
            $scope.templateUserEdit = response.data;
        });
    };

    $scope.updateUserInfo = function (isShownEditableTextbox, user, fieldNameToChange, newValue) {

        if (!isShownEditableTextbox || user == $scope.EMPTY) {
            $window.alert('error');
            return;
        }

        $http({
            method: 'POST',
            url: '/Account/UpdateUserInfo',
            params: { userName: user.UserName, field: fieldNameToChange, newValue: newValue },
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).
                then(function (response) {
                    //success
                }, function (response) {
                    //error
                    $window.alert('error');
                });
    }

    $scope.setDefaultCompany = function () {
        var defaultcompanyId = null;
        if ($scope.userModel.selectedRole == 'Customer') {
            defaultcompanyId = $.grep($scope.companies, function (e) { return e.isCustomer == true; })[0];
        }
        if ($scope.userModel.selectedRole == 'Producer') {
            defaultcompanyId = $.grep($scope.companies, function (e) { return e.isCustomer == false; })[0];
        }
        if ($scope.userModel.selectedRole == 'Admin') {
            $scope.userModel.companyId = null;
            return;
        }
        $scope.userModel.companyId = defaultcompanyId;
    };

    $scope.addUser = function () {
        var resultUser = $scope.userModel;
        if (isNaN(parseFloat(resultUser.companyId)) && !isFinite(resultUser.companyId)) {
            resultUser.companyId = resultUser.companyId.id;
        }

        $http({
            method: 'POST',
            url: '/Home/RegisterFormeo',
            data: {
                model: resultUser
            }
        }).
            then(function (response) {
                if ($scope.userModel.selectedRole == 'Customer') {
                    $scope.customers.push(JSON.parse(response.data));
                }
                if ($scope.userModel.selectedRole == 'Producer') {
                    $scope.producers.push(JSON.parse(response.data));
                }

                $scope.cleanUserModel();

            }, function (response) {
                alert("There is something wrong with adding user");
                // called asynchronously if an error occurs
                // or server returns response with an error status.
            });
    };

    $scope.getCompanyNameById = function (companyId) {
        return $.grep($scope.companies, function (e) { return e.id == companyId; })[0].companyName;
    }

    $scope.removeUser = function (user, collectionToHandle) {
        if (user != $scope.EMPTY) {
            $http({
                method: 'POST',
                url: '/Home/RemoveUser',
                params: { email: user.Email },
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).
                then(function (response) {
                    var index = collectionToHandle.indexOf(user);
                    collectionToHandle.splice(index, 1);
                    $scope.selectClientMenu('/AddUser');
                }, function (response) {
                    alert("There is something wrong with deleting user");
                    // called asynchronously if an error occurs
                    // or server returns response with an error status.
                });
        }
    };


    $scope.removeCompany = function (company, collectionToHandle) {
        if (company != $scope.EMPTY) {
            $http({
                method: 'POST',
                url: '/Home/RemoveCompany',
                params: { companyId: company.id },
                //headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).
                then(function (response) {
                    var index = collectionToHandle.indexOf(company);
                    collectionToHandle.splice(index, 1);

                    var usersToDelete = JSON.parse(response.data);
                    //var producersToDelete = [];
                    //var customersToDelete = [];
                    usersToDelete.forEach(function (entry) {

                        $scope.producers = $scope.producers.filter(function (obj) {
                            return obj.Id !== entry;
                        });

                        $scope.customers = $scope.customers.filter(function (obj) {
                            return obj.Id !== entry;
                        });

                        //producersToDelete.push($.grep($scope.producers, function (e) { return e.Id == entry; })[0]);
                    });

                }, function (response) {
                    alert("There is smth wrong with deleting company");
                    // called asynchronously if an error occurs
                    // or server returns response with an error status.
                });
        }
    };


    $scope.selectUser = function (user) {
        if (user != $scope.EMPTY) {
            $scope.selectedUser = user;
        }
    };

    $scope.showEditCompanyModal = function () {

        var modalInstance = $modal.open({
            animation: true,
            template: $scope.templateCompanyEdit,
            controller: 'companyEditController',
            size: "editCompany",
            resolve: {
                company: function () {
                    return $scope.selectedCompany;
                }
            }
        });
        modalInstance.result.then(function (selectedItem) {
            $scope.selectedCompany = selectedItem;
            var result = $.grep($scope.companies, function (e) { return e.id == selectedItem.id; })[0];
            result.companyName = selectedItem.companyName;
            result.country = selectedItem.country;
            result.orgNumber = selectedItem.orgNumber;
            result.taxNumber = selectedItem.taxNumber;
        }, function () {
            //$log.info('Modal dismissed at: ' + new Date());
        });
    };


    $scope.showEditUserModal = function () {
        var oldRoleName = $scope.selectedUser.SelectedRole;
        var modalInstance = $modal.open({
            animation: true,
            template: $scope.templateUserEdit,
            controller: 'userEditController',
            size: "editCompany",
            resolve: {
                user: function () {
                    return $scope.selectedUser;
                },
                companies: function () {
                    return $scope.companies;
                }
            }
        });

        modalInstance.result.then(function (selectedItem) {
            if (selectedItem.SelectedRole == oldRoleName) {
                $scope.selectedUser = selectedItem;
                var result = null;
                if (selectedItem.SelectedRole == 'Producer') {
                    result = $.grep($scope.producers, function (e) { return e.Id == selectedItem.Id; })[0];
                }
                else {
                    result = $.grep($scope.customers, function (e) { return e.Id == selectedItem.Id; })[0];
                }

                result.Company.Id = selectedItem.Company.Id;
                result.Company.CompanyName = selectedItem.Company.CompanyName;
                result.Email = selectedItem.Email;
                result.Address = selectedItem.Address;
                result.Postal = selectedItem.Postal;
                result.City = selectedItem.City;
                result.Country = selectedItem.Country;
            }
            else {
                if ($scope.selectedUser.SelectedRole == 'Producer') {
                    var index = $scope.producers.indexOf($scope.selectedUser);
                    $scope.producers.splice(index, 1);
                    $scope.customers.push(selectedItem);
                }
                else {
                    var index = $scope.customers.indexOf($scope.selectedUser);
                    $scope.customers.splice(index, 1);
                    $scope.producers.push(selectedItem);
                }
                $scope.selectedUser = selectedItem;
            }
        }, function () {
            //$log.info('Modal dismissed at: ' + new Date());
        });
    };

    $scope.selectCompany = function (company) {
        if (company != $scope.EMPTY) {
            $scope.selectedCompany = company;
        }
    }

    $scope.isMenuTypeIs = function (type) {
        return $scope.menuType === type;
    };

    $scope.selectMainMenu = function (item) {
        $scope.selectedMainMenu = item;
    };

    $scope.isActiveMainMenu = function (item) {
        return $scope.selectedMainMenu === item;
    };

    $scope.selectClientMenu = function (item) {
        $scope.selectedClientsMenu = item;
    };

    $scope.selectCompaniesMenu = function (item) {
        $scope.selectedCompaniesMenu = item;
    };

    $scope.isActiveCLientsMenu = function (item) {
        return $scope.selectedClientsMenu === item;
    };

    $scope.isActiveCompaniesMenu = function (item) {
        return $scope.selectedCompaniesMenu === item;
    };

    $scope.addCompany = function () {
        $http({
            method: 'POST',
            url: '/Home/CreateCompany',
            headers: { 'Content-Type': 'application/json;' },
            data: {
                'Name': $scope.companyModel.companyName,
                'OrgNumber': $scope.companyModel.orgNumber,
                'TaxNumber': $scope.companyModel.taxNumber,
                'Country': $scope.companyModel.country,
                'IsCustomer': $scope.companyModel.isCustomer ? true : false
            }
        }).
           then(function (response) {
               $scope.companies.push(JSON.parse(response.data));
               $scope.cleanCompanyModel();
           }, function (response) {
               $window.alert('error creating company');
           });
    };

    $scope.cleanUserModel = function () {
        $scope.userModel = {
            username: '',
            password: '',
            email: '',
            address: '',
            postal: '',
            country: '',
            companyId: '',
            selectedRole: 'Admin'
        };
    };

    $scope.cleanCompanyModel = function () {
        $scope.companyModel = {
            id: '',
            orgNumber: '',
            country: '',
            taxNumber: '',
            companyName: '',
            isCustomer: false
        };
    }
}