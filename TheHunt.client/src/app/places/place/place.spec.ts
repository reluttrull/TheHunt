import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Place } from './place';

describe('Place', () => {
  let component: Place;
  let fixture: ComponentFixture<Place>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Place]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Place);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
