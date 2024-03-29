﻿/// <reference path="../app.ts" />

module app.services {

    export class AuthService implements IService {
        q;
        http;
        authentication = {
            isAuth: false,
            userName: ""
        };

        constructor($http: ng.IHttpService, $q) {
            this.q = $q;
            this.http = $http;
        }

        register = registration => {
            this.logout();

            return this.http.post('/api/account/register', registration).then(response => response);
        };

        login = loginData => {
            var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

            var deferred = this.q.defer();

            this.http.post('/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(response => {
                sessionStorage.setItem('authorizationData', JSON.stringify({ token: response.access_token, userName: loginData.userName }));
                this.authentication.isAuth = true;
                this.authentication.userName = loginData.userName;

                deferred.resolve(response);

            }).error((err, status) => {
                this.logout();
                deferred.reject(err);
            });

            return deferred.promise;
        };

        logout = () => {
            sessionStorage.removeItem('authorizationData');

            this.authentication.isAuth = false;
            this.authentication.userName = "";
        };

        restore = () =>
        {
            var authData = sessionStorage.getItem('authorizationData');
            if (authData)
            {
                this.authentication.isAuth = true;
                this.authentication.userName = authData.userName;
            }
        };
    };
}

angular.module('app').factory('authService', ['$http', '$q', ($http, $q) => new app.services.AuthService($http, $q)]);
//app.registerService('AuthService', ['$http', '$q']);