-- グラビディア・ウェルズ

function RegisterAbilities()
	-- placed on VC
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.PlacedOnVC)
	ability1.SetTrigger("PlacedOnVCTrigger")
	ability1.SetActivation("PlacedOnVC")
	-- ACT
	local ability2 = NewAbility(GetID())
	ability2.SetTiming(a.OnACT)
	ability2.SetLocation(l.PlayerRC)
	ability2.SetProperty(p.OncePerTurn)
	ability2.SetCondition("ACTCondition")
	ability2.SetCanFullyResolve("ACTCanFullyResolve")
	ability2.SetCost("ACTCost")
	ability2.SetActivation("ACT")
end

function PlacedOnVCTrigger()
	return obj.LastPlacedOnVC() 
end

function PlacedOnVC()
	obj.LookAtTopOfDeck(7)
	obj.Store(obj.ChooseAddToHand({q.Location, l.Looking, q.Other, o.Meteorite, q.Min, 0}))
	obj.Reveal({q.Location, l.Stored})
	obj.Shuffle()
end

function ACTCondition()
	return obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt03_005")})
end

function ACTCanFullyResolve()
	return obj.Exists({q.Location, l.Hand, q.Other, o.Meteorite})
end

function ACTCost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function ACT()
	obj.ChoosePutIntoOrderZone({q.Location, l.Hand, q.Other, o.Meteorite, q.Count, 2, q.Min, 0})
	obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.This}, cs.BonusPower, 2000, p.UntilEndOfTurn)
end