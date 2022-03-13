--双子のダイアフルドール リリミ

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetCondition("OnPlaceCondition")
	ability1.SetCost("OnPlaceCost")
	ability1.SetCanFullyResolve("OnPlaceCanFullyResolve")
	ability1.SetActivation("OnPlace")
	-- on retire
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetProperty(p.Glitter)
	ability2.SetTiming(a.OnRetire)
	ability2.SetMovedFrom(l.GC)
	ability2.SetTrigger("OnRetireTrigger")
	ability2.SetCondition("OnRetireCondition")
	ability2.SetActivation("OnRetire")
	ability2.SetProperty(p.NotMandatory)
end

function OnPlaceTrigger()
	return obj.IsApplicable()
end

function OnPlaceCondition()
	return obj.Exists({q.Location, l.PlayerRC, q.NameContains, obj.LoadName("Rarami")})
end

function OnPlaceCost(check)
	if check then return obj.CanSB(2) end
	obj.SoulBlast(2)
end

function OnPlaceCanFullyResolve()
	return obj.Exists({q.Location, l.EnemyRC, q.Other, o.CanChoose})
end

function OnPlace()
	obj.ChooseAddToSoul({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1})
end

function OnRetireTrigger()
	return obj.IsApplicable()
end

function OnRetireCondition()
	return obj.GetNumberOf("Filter", {l.Units}) > 0 and obj.IsSameZone()
end

function Filter(id)
	return obj.IsPlayer(id) and obj.IsVanguard(id) and obj.NameContains(id, obj.LoadName("Tamayura")) and obj.HasProperty(id, p.IsGlitter)
end

function OnRetire()
	obj.AddToSoul({q.Other, o.ThisFieldID})
end