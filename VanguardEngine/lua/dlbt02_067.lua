-- MiMish アズハチル

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC, l.VC)
	ability1.SetActivation("Cont")
	-- on ride
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnRide)
	ability2.SetTrigger("OnRideTrigger")
	ability2.SetCondition("OnRideCondition")
	ability2.SetActivation("OnRide")
end

function Cont()
	if obj.GetNumberOf("ContFilter", {l.Units}) > 0 then
		obj.AddCardState("MakeFriendFilter", {l.Units}, cs.Friend, p.Continuous)
	end
end

function ContFilter(id)
	return not obj.IsThis(id) and obj.IsPlayer(id) and obj.IsSameColumn(id)
end

function MakeFriendFilter(id)
	return obj.IsThis(id) and obj.IsPlayer(id) and obj.IsSameColumn(id)
end

function OnRideTrigger()
	return obj.WasRodeUponBy(obj.GetNameFromCardID("dlbt02_038"))
end

function OnRideCondition()
	return obj.CanSuperiorCall("SuperiorCallFilter", {l.Soul})
end

function SuperiorCallFilter(id)
	return obj.IsApplicable(id)
end

function OnRide()
	obj.SuperiorCall("SuperiorCallFilter", {l.Soul}, 1, 1)
end