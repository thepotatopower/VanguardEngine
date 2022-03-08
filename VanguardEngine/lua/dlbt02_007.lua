-- Hoppin'Stellar メルティ

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetProperty(p.Friend)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Cont")
	-- on place
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.PlacedOnRC)
	ability2.SetTrigger("OnPlaceTrigger")
	ability2.SetCost("OnPlaceCost")
	ability2.SetCanFullyResolve("OnPlaceCanFullyResolve")
	ability2.SetActivation("OnPlace")
end

function Cont()
	if obj.GetNumberOf("ContFilter", {l.Units}) > 0 then
		obj.AddCardState("FriendFilter", {l.Units}, cs.Friend, p.Continuous)
	end
end

function ContFilter(id)
	return not obj.IsThis(id) and obj.IsPlayer(id) and obj.IsSameColumn(id)
end

function FriendFilter(id)
	return obj.IsPlayer(id) and obj.IsSameColumn(id)
end

function OnPlaceTrigger()
	return obj.IsApplicable() and obj.PlayerMainPhase()
end

function OnPlaceCost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function OnPlaceCanFullyResolve()
	return obj.CanSuperiorCall("OnPlaceFilter", {l.Hand})
end

function OnPlaceFilter(id)
	return obj.GradeOf(id) <= obj.VanguardGrade()
end

function OnPlace()
	obj.Store(obj.SuperiorCall("OnPlaceFilter", {l.Hand}, 1, 1))
	if obj.GetNumberOf({q.Location, l.Stored}) > 0 then
		obj.Draw(1)
	end
end