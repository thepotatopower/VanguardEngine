-- 天声の代弁者 ヘリュエル

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetProperty(p.WhiteWings)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetMovedFrom(l.Hand)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable() and obj.PlayerMainPhase()
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function CanFullyResolve()
	return obj.CanSuperiorCall("SCFilter", {l.Drop}, FL.OpenCircle)
end

function SCFilter(id) 
	return obj.HasProperty(id, p.WhiteWings) and not obj.NameIs(id, obj.GetNameFromCardID("dlbt02_023"))
end

function Activation()
	obj.SuperiorCall("SCFilter", {l.Drop}, 1, 1, true, "", false, {FL.OpenCircle})
end