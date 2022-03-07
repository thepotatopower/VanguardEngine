-- 次のステップ ラプラム

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.NotMandatory)
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Astesice")}) and 
	obj.CanSuperiorCall("SCFilter", {l.Soul}, FL.OpenCircle)
end

function SCFilter(id)
	return obj.GradeOf(id) <= 2 and obj.NameContains(id, obj.LoadName("Astesice"))
end

function Activation()
	obj.Store(obj.SuperiorCall("SCFilter", {l.Soul}, 1, 1, false, "", false, {FL.OpenCircle}))
	if obj.IsPlayerTurn() and obj.IsBattlePhase() then
		obj.Stand({q.Location, l.Stored})
	end
end