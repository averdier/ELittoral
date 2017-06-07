using System;

using ELittoral.Helpers;
using ELittoral.Models;
using System.Collections.Generic;

namespace ELittoral.ViewModels
{
    public class HomeViewModel : Observable
    {
        private InstructionItem _selectedInstructionItem;

        /// <summary>
        /// The instructional items.
        /// </summary>
        public IList<InstructionItem> InstructionItems { get; private set; }

        public HomeViewModel()
        {
            InitializeInstructionItems();
        }

        /// <summary>
        /// Gets or sets the current instructional item.
        /// </summary>
        public InstructionItem SelectedInstructionItem
        {
            get { return _selectedInstructionItem; }
            set
            {
                if (value != _selectedInstructionItem)
                {
                    _selectedInstructionItem = value;
                    OnPropertyChanged(nameof(SelectedInstructionItem));
                }
            }
        }

        private void InitializeInstructionItems()
        {
            InstructionItems = new List<InstructionItem>();


            InstructionItems.Add(new InstructionItem(
                "ELittoral",
                "Donec at neque id justo consectetur iaculis. Integer mollis vitae ligula a facilisis. Maecenas at tincidunt tellus. Pellentesque in cursus libero. Nunc ante nunc, rhoncus interdum est non, finibus dictum erat.", 
                new Uri("ms-appx:///Assets/Home/logo_drone.png")));

            InstructionItems.Add(new InstructionItem(
                "Présentation",
                "Duis pharetra, purus non hendrerit volutpat, elit tortor porttitor diam, eu vehicula nibh nisi eget nibh. In metus ex, vehicula a egestas id, suscipit a nibh. Maecenas ipsum leo, blandit sit amet odio ac, facilisis consequat enim. Vivamus porttitor egestas velit, sed elementum neque. Vivamus dictum libero nec enim tempor, eu suscipit massa molestie. Sed eu lectus euismod nunc interdum posuere ut et mauris. Nam vitae ullamcorper ligula, ut elementum risus. Pellentesque laoreet libero ac purus volutpat aliquet. Sed ut consectetur lorem, eu lobortis urna. Quisque tristique odio non massa tincidunt, sit amet mollis sapien lobortis. Quisque malesuada metus eu tortor placerat, eget condimentum mauris condimentum. Duis et pellentesque turpis, a sollicitudin felis. Cras eu egestas orci, nec maximus tellus. Sed pretium lacus a libero auctor convallis.", 
                new Uri("ms-appx:///Assets/Home/drone.png")));

            InstructionItems.Add(new InstructionItem(
                "Delferiere Julien",
                "Cras placerat ut mauris id hendrerit. Maecenas quis felis et dolor fermentum finibus id id ex. Nullam quis efficitur nulla, nec commodo ex. Duis eleifend tortor ac feugiat ultrices. Suspendisse in enim vel elit viverra varius. Curabitur in laoreet justo, sit amet ullamcorper justo. Aenean sollicitudin est eu mi lobortis, non mollis nisi iaculis. Fusce iaculis nunc purus. Morbi finibus quis mauris ultricies dignissim. Integer interdum egestas purus sit amet posuere. Maecenas interdum ac diam in egestas. Phasellus ut feugiat sem, ac pharetra sapien.", 
                new Uri("ms-appx:///Assets/Home/ios.png")));

            InstructionItems.Add(new InstructionItem(
                "Sagbo Maurel",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur finibus vestibulum ipsum, at volutpat dolor venenatis vel. Curabitur sodales non felis feugiat commodo. Aliquam a posuere risus. Proin tortor est, lacinia eget nisi non, aliquam placerat nulla. Etiam porttitor egestas tempus. Integer aliquam suscipit leo quis commodo. Fusce blandit neque et nunc porta finibus.", 
                new Uri("ms-appx:///Assets/Home/android.png")));

            InstructionItems.Add(new InstructionItem(
                "Verdier Arthur",
                "Sed congue a libero non iaculis. Donec pharetra lectus quis lorem efficitur vulputate. Donec interdum ipsum felis, mattis laoreet dui dictum a. Suspendisse vitae orci est. Aenean quis ante eget tellus viverra faucibus eget vel elit. Sed dolor ante, commodo quis vestibulum id, imperdiet sit amet urna. Proin ornare feugiat dignissim. Nulla facilisi. Donec sit amet est ac metus ullamcorper rutrum. Proin at dapibus nunc. Mauris et elit at tellus tempus volutpat ut sit amet felis. Donec lacinia metus ac sem scelerisque, eu facilisis arcu sollicitudin. Aliquam at arcu urna.", 
                new Uri("ms-appx:///Assets/Home/win10.png")));

            InstructionItems.Add(new InstructionItem(
                "Remerciements",
                "Praesent non leo vitae nisl consequat mollis sed a dui. Nulla consectetur erat ac elit commodo dictum. Phasellus eu risus a lectus dignissim ullamcorper id nec quam. Maecenas tellus orci, tempor non purus sodales, aliquet accumsan velit. Vivamus scelerisque sapien sed fermentum euismod. Mauris volutpat dui eros, ac consequat magna accumsan eget. Duis faucibus sapien at justo tincidunt varius.", 
                new Uri("ms-appx:///Assets/Home/remerciements.png")));
        }
    }
}
