"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var testing_1 = require("@angular/core/testing");
var building_service_1 = require("./building.service");
describe('BuildingService', function () {
    beforeEach(function () { return testing_1.TestBed.configureTestingModule({}); });
    it('should be created', function () {
        var service = testing_1.TestBed.get(building_service_1.BuildingService);
        expect(service).toBeTruthy();
    });
});
//# sourceMappingURL=building.service.spec.js.map