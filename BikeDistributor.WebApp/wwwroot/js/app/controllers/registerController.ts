/// <reference path="../app.ts" />

module app.controllers {

    export class RegisterController implements IController {

        constructor($scope, $location: ng.ILocationService, authService: app.services.AuthService, $timeout) {
            $scope.savedSuccessfully = false;
            $scope.message = "";

            $scope.registration = {
                userName: "",
                password: "",
                confirmPassword: ""
            };

            var startTimer = () => {
                var timer = $timeout(() => {
                    $timeout.cancel(timer);
                    $location.path('/login');
                }, 2000);
            }

            $scope.signUp = () => {
                authService.register($scope.registration).then(response => {
                    $scope.savedSuccessfully = true;
                    $scope.message = "User has been registered successfully, you will be redicted to login page in 2 seconds.";
                    startTimer();
                },
                    response => {
                        var errors = [];
                        for (var key in response.data.modelState) {
                            for (var i = 0; i < response.data.modelState[key].length; i++) {
                                errors.push(response.data.modelState[key][i]);
                            }
                        }
                        $scope.message = "Failed to register user due to:" + errors.join(' ');
                    });
            };
        }
    };
}

app.registerController('RegisterController', ['$scope', '$location', 'authService', '$timeout']);