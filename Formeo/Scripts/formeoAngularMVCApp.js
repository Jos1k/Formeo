var formeoAngularMVCApp = angular.module('formeoAngularMVCApp', ['ui.bootstrap']);

formeoAngularMVCApp.controller('commonController', commonController);
formeoAngularMVCApp.controller('adminPageController', adminPageController);
formeoAngularMVCApp.controller('customerPageController', customerPageController);
formeoAngularMVCApp.controller('producerPageController', producerPageController);

formeoAngularMVCApp.controller('layOrderPartialController', layOrderPartialController);
formeoAngularMVCApp.controller('layOrderConfirmPartialController', layOrderConfirmPartialController);
formeoAngularMVCApp.controller('addProductsPartialController', addProductsPartialController);
formeoAngularMVCApp.controller('bidProductPartialController', bidProductPartialController);
formeoAngularMVCApp.controller('uploadProductController', uploadProductController);