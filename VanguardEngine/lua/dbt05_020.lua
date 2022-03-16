--忍妖 フォークテイル

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.NotMandatory)
end

function Trigger()
	return obj.IsApplicable()
end

function Activation()
	obj.LookAtTopOfDeck(7)
	obj.Select("Filter", {l.Looking}, 1, 0, {}, -1)
	obj.AddToSoul({q.Location, l.Selected})
	obj.Shuffle()
end

function Filter(id)
	return obj.IsUnitType(id, u.Normal) and (obj.NameContains(id, obj.LoadName("Stealth")) or obj.HasProperty(id, p.Glitter) or obj.HasProperty(id, p.IsGlitter))
end