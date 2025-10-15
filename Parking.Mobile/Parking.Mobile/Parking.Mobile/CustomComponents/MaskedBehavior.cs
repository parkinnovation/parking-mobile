using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;

namespace Parking.Mobile.CustomComponents
{
    public class MaskedBehavior : Behavior<Entry>
    {

        private string textContent = "";
        private string _mask = "";
        public string Mask
        {
            get => _mask;
            set
            {

                _mask = value;
                SetPositions();
            }
        }

        private void SetKeyboard(Entry entry)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                int length = !String.IsNullOrEmpty(this.textContent) ? this.textContent.Length : 0;

                bool existsMask = (from l in _positions.Keys where l == length select l).Count() > 0;

                if (existsMask)
                    length++;

                if (length < Mask.Length)
                {
                    if (Mask[length] == 'X' || Mask[length] == 'A')
                    {
                        entry.Keyboard = Keyboard.Text;
                    }
                    else if (Mask[length] == '9')
                    {
                        entry.Keyboard = Keyboard.Numeric;
                    }
                    else
                    {
                        entry.Keyboard = Keyboard.Text;
                    }
                }
                else
                {
                    entry.Keyboard = Keyboard.Text;
                }
            });
        }

        protected override void OnAttachedTo(Entry entry)
        {
            SetKeyboard(entry);

            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        IDictionary<int, char> _positions;

        void SetPositions()
        {
            if (string.IsNullOrEmpty(Mask))
            {
                _positions = null;
                return;
            }

            var list = new Dictionary<int, char>();
            for (var i = 0; i < Mask.Length; i++)
                if (Mask[i] != 'X' && Mask[i] != '9' && Mask[i] != 'A')
                    list.Add(i, Mask[i]);

            _positions = list;
        }

        private string ValidateMask(string newText, string oldText)
        {
            string correctText = newText;

            if (!string.IsNullOrEmpty(newText))
            {
                for (int i = 0; i < newText.Length; i++)
                {
                    if (i < Mask.Length)
                    {
                        if (Mask[i] == 'A')
                        {
                            if (!Char.IsLetter(newText[i]))
                            {
                                correctText = oldText;
                                break;
                            }
                        }

                        if (Mask[i] == '9')
                        {
                            if (!Char.IsDigit(newText[i]))
                            {
                                correctText = oldText;
                                break;
                            }
                        }

                        if (Mask[i] == 'X')
                        {
                            if (!Char.IsLetterOrDigit(newText[i]))
                            {
                                correctText = oldText;
                                break;
                            }
                        }
                    }
                }
            }

            return correctText;
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var entry = sender as Entry;


            var text = entry.Text;



            if (string.IsNullOrWhiteSpace(text) || _positions == null)
                return;

            if (text.Length > _mask.Length)
            {
                entry.Text = text.Remove(text.Length - 1);
                return;
            }

            foreach (var position in _positions)
                if (text.Length >= position.Key + 1)
                {
                    var value = position.Value.ToString();
                    if (text.Substring(position.Key, 1) != value)
                        text = text.Insert(position.Key, value);
                }

            if (entry.Text != text)
                entry.Text = text;

            this.textContent = text;

            entry.Text = ValidateMask(this.textContent, args.OldTextValue);

            SetKeyboard(entry);
        }
    }
}

