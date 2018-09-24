import { Component, EventEmitter, OnInit, Input, Output } from '@angular/core';

@Component({
  selector: 'app-daypicker',
  templateUrl: './daypicker.component.html',
  styleUrls: ['./daypicker.component.css']
})
export class DaypickerComponent implements OnInit {

  days = [
    {name: "Sunday", checked: false},
    {name: "Monday", checked: false},
    {name: "Tuesday", checked: false},
    {name: "Wednesday", checked: false},
    {name: "Thursday", checked: false},
    {name: "Friday", checked: false},
    {name: "Saturday", checked: false},
  ]

  private _selectedDays = [];
 
  @Input()
  set selectedDays(inputdays: string[]) {
    if (inputdays === null) {
      this._selectedDays = [];
    } else {
      this._selectedDays = inputdays
    }
    for (var i = 0; i < this.days.length; i++) {
      this.days[i].checked = this._selectedDays.includes(this.days[i].name)
    }
  }
  get selectedDays(): string[] { return this._selectedDays; }

  @Output() changed = new EventEmitter<string[]>();

  onchange(name: string, checked: boolean) {
    var index = this.days.findIndex(x => x.name === name)
    this.days[index].checked = checked;
    this.changed.emit(
      this.days.filter(x => x.checked).map(
        y => y.name
      )
    );
  }

  constructor() { }

  ngOnInit() {
  }

}
