import { ThisReceiver } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {

  member: Member | undefined;

  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];

  constructor(private membersService: MembersService, private route: ActivatedRoute) { }

  ngOnInit(): void {

    this.loadMember();

    this.galleryOptions = [

      {
        width: '500px',
        height: '500px',
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        imagePercent: 100,
        preview: false
      }
    ];

   
  }

  getImages() {

    if (!this.member) return [];

    for (const photo of this.member.photos) {
      this.galleryImages.push({

        small: photo.url,
        medium: photo.url,
        big: photo.url,
      });
    }
    return this.galleryImages;

  }

  loadMember() {
    const username = this.route.snapshot.params["username"];

    if (!username) return;

    this.membersService.getMember(username).subscribe({
      next: member => {
        this.member = member;

        this.getImages();
      },
      error: (error: any) => { console.log(error); }
    })
  }

}
