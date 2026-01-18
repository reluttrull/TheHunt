import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NearbyPlacesMap } from './nearby-places-map';

describe('NearbyPlacesMap', () => {
  let component: NearbyPlacesMap;
  let fixture: ComponentFixture<NearbyPlacesMap>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NearbyPlacesMap]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NearbyPlacesMap);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
