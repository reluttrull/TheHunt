import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyPlacesMap } from './my-places-map';

describe('MyPlacesMap', () => {
  let component: MyPlacesMap;
  let fixture: ComponentFixture<MyPlacesMap>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MyPlacesMap]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MyPlacesMap);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
