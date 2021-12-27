-- 天剣の騎士 フォート

function RegisterAbilities()
	-- on ride
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRide)
	ability1.SetTrigger("OnRideTrigger")
	ability1.SetCost("OnRideCost")
	ability1.SetActivation("OnRide")
	-- ACT
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnACT)
	ability2.SetProperty(p.OncePerTurn)
	ability2.SetLocation(l.RC)
	ability2.SetCost("ACTCost")
	ability2.SetCanFullyResolve("ACTCanFullyResolve")
	ability2.SetActivation("ACT")
end

function OnRideTrigger()
	return obj.WasRodeUponBy(obj.GetNameFromCardID("dsd03_002"))
end

function OnRideCost(check)
	if check then return obj.CanReveal({q.Location, l.Hand, q.Grade, 3, q.Count, 2}) end
	obj.ChooseReveal({q.Location, l.Hand, q.Grade, 3, q.Count, 2})
end

function OnRide()
	obj.RevealFromDeck(1)
	obj.SuperiorCall({q.Location, l.Revealed, q.Other, o.Unit})
	obj.AddToDrop({q.Location, l.Revealed})
end

function ACTCost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function ACTCanFullyResolve()
	obj.Exists({q.Location, l.PlayerVC, q.Grade, 3})
end

function ACT()
	obj.Select({q.Location, l.PlayerVC, q.Grade, 3, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end