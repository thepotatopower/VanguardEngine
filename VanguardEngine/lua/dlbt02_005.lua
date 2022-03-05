-- 紡ぎ重ねる追走曲 ディートリンデ

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
	-- on end of battle
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnBattleEnds)
	ability2.SetLocation(l.RC)
	ability2.SetTrigger("OnBattleEndsTrigger")
	ability2.SetCondition("OnBattleEndsCondition")
	ability2.SetActivation("OnBattleEnds")
end

function OnPlaceTrigger()
	return obj.IsApplicable()
end

function OnPlaceCondition()
	return obj.GetNumberOf("FeltyVC", {l.Units}) > 0
end

function FeltyVC(id)
	return obj.IsPlayer(id) and obj.IsVanguard(id) and obj.NameIs(id, obj.GetNameFromCardID("dlbt01_008"))
end

function OnPlaceCost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function OnPlaceCanFullyResolve()
	return obj.CanSuperiorCall("OnPlaceFilter", {l.Drop}, {FL.OpenCircle})
end

function OnPlace()
	obj.SuperiorCall("OnPlaceFilter", {l.Drop}, 1, 1, true, "", false, {FL.OpenCircle})
end

function OnPlaceFilter(id)
	return obj.IsRace(id, r.Ghost) and obj.IsUnitType(id, u.Normal) and not obj.NameIs(id, obj.GetNameFromCardID("dlbt02_005"))
end

function OnBattleEndsTrigger()
	return obj.IsApplicable()
end

function OnBattleEndsCondition()
	return obj.IsSameZone()
end

function OnBattleEnds()
	obj.SendToBottom("SendToBottomFilter", {l.Units})
end

function SendToBottomFilter(id)
	return obj.IsApplicable(id)
end