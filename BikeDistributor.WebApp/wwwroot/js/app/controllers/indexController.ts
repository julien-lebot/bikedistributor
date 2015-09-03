/// <reference path="../app.ts" />

module app.controllers {
    export class IndexController implements IController {

        constructor($scope, $location: ng.ILocationService, authService: app.services.AuthService)
        {
            $scope.logOut = () =>
            {
                authService.logout();
                $location.path('/');
            };

            $scope.authentication = authService.authentication;
        }
    };
}

app.registerController('IndexController', ['$scope', '$location', 'authService']);