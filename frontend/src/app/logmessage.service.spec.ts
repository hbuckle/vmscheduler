import { TestBed } from '@angular/core/testing';

import { LogMessageService } from './logmessage.service';

describe('LogMessageService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: LogMessageService = TestBed.get(LogMessageService);
    expect(service).toBeTruthy();
  });
});
