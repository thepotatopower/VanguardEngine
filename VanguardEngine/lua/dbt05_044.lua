-- 研究は順調 エバ

function RegisterAbilities()
	-- on ride
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnVC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("OnRide")
	-- glitter
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetProperty(p.IsGlitter)
end

function Trigger()
	return obj.PlacedByRidingOn(obj.GetNameFromCardID("dbt05_070"))
end

function Condition()
	return obj.GetNumberOf("Filter", {l.Deck}) > 0
end

function Filter(id)
	return obj.IsOrderType(id, ot.Research)
end

function OnRide()
	obj.Search("Filter", {l.Deck}, 1, 0);
end