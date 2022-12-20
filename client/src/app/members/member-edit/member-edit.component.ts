import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {

  member: Member | undefined;
  user: User | null = null;

  // Need to use FormId for reset. editForm is the child template of current Component. In order to access editForm we have used that.
  @ViewChild('editForm') editForm: NgForm | undefined;

  @HostListener('window:beforeunload', ['$event']) handleBrowserButton(event: any) {

    if (this.editForm?.dirty) {

      event.returnValue = true;
    }
  }

  constructor(private membersService: MembersService, private accountService: AccountService, private toastr: ToastrService) {

    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: (user) => {
        this.user = user;
      },
      error: (error) => { console.error(error); }

    });
  }

  ngOnInit(): void {

    this.loadMember();
  }

  loadMember() {
    if (!this.user) return;

    this.membersService.getMember(this.user.userName).subscribe({
      next: (member) => {
        this.member = member;
      },
      error(err) {
        console.error(err);
      },
    })
  }

  updateMember() {

    this.membersService.updateMember(this.member!).subscribe({
      next: _ => {

        this.toastr.success("Profile updated sucessfully");
        this.editForm?.reset(this.member);
      }
    });
  }

}
