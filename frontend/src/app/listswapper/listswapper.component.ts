import { Component, EventEmitter, OnInit, Input, Output } from '@angular/core';

@Component({
  selector: 'app-listswapper',
  templateUrl: './listswapper.component.html',
  styleUrls: ['./listswapper.component.css']
})
export class ListswapperComponent implements OnInit {

  @Input() listalpha: string[];
  @Input() listomega: string[];
  @Output() changedalpha = new EventEmitter<string[]>();
  @Output() changedomega = new EventEmitter<string[]>();

  constructor() { }

  ngOnInit() {
  }

  dropalpha(item: string) {
    var index = this.listomega.indexOf(item, 0);
    if (index > -1) {
      let newlistomega = this.listomega.slice(0);
      newlistomega.splice(index, 1);
      this.listomega = newlistomega;
    }
    let newlistalpha = this.listalpha.slice(0);
    newlistalpha.push(item);
    this.listalpha = newlistalpha;
  }

  dropomega(item: string) {
    var index = this.listalpha.indexOf(item, 0);
    if (index > -1) {
      let newlistalpha = this.listalpha.slice(0);
      newlistalpha.splice(index, 1);
      this.listalpha = newlistalpha;
    }
    let newlistomega = this.listomega.slice(0);
    newlistomega.push(item);
    this.listomega = newlistomega;
  }

}
