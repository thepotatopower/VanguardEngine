-- グラビディア・プシブラム

function RegisterAbilities()
	-- on attack hits
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttackHits)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit()
end

function Condition()
	return obj.Exists({q.Location, l.Deck, q.Other, o.Meteorite})
end

function Activation()
	obj.ChoosePutIntoOrderZone({q.Location, l.Deck, q.Other, o.Meteorite, q.Count, 1, q.Min, 0})
	obj.Shuffle()
end