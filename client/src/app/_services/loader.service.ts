import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class LoaderService {

  requestCount = 0;

  constructor(private spinner: NgxSpinnerService) { }

  loadSpinner() {
    this.requestCount++;
    
    this.spinner.show(undefined, {
      type: 'line-scale-party',
      bdColor: 'rgba(255, 255, 255, 0)',
      color: '#333333'
    });
  }

  unLoadSpinner() {
    this.requestCount--;

    if (this.requestCount <= 0) {

      this.requestCount =0;
      this.spinner.hide();
    }
  }
}
