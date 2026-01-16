import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecordLocation } from './record-location';

describe('RecordLocation', () => {
  let component: RecordLocation;
  let fixture: ComponentFixture<RecordLocation>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecordLocation]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecordLocation);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
