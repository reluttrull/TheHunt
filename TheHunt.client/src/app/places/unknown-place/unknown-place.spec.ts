import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UnknownPlace } from './unknown-place';

describe('UnknownPlace', () => {
  let component: UnknownPlace;
  let fixture: ComponentFixture<UnknownPlace>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UnknownPlace]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UnknownPlace);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
