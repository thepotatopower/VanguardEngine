-- 天令竜 イグザルート・ドラゴン

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Bastion")})
end

function Cost(check)
	if check then return obj.CanCB(1) and obj.CanSB(1) end
	obj.CounterBlast(1)
	obj.SoulBlast(1)
end

function CanFullyResolve()
	return obj.CanSuperiorCallToSpecificCircle({q.Location, l.Deck, q.Name, obj.GetNameFromCardID("dss02_029"), q.Count, 1}, obj.GetColumn({q.Other, o.ThisFieldID}))
end

function Activation()
	obj.SuperiorCallToSpecificCircle({q.Location, l.Deck, q.Name, obj.GetNameFromCardID("dss02_029"), q.Count, 1, q.Min, 0}, obj.GetColumn({q.Other, o.ThisFieldID}))
	obj.Shuffle()
end