-- 眠りからの目覚め ロロワ

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnVC)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.NotMandatory)
	-- glitter
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetProperty(p.IsGlitter)
end

function Trigger()
	return obj.PlacedByRidingOn(obj.GetNameFromCardID("dbt05_090"))
end

function Activation()
	obj.SuperiorCall({}, {}, 1, 1, "dbt05_t02", {}, {FL.PlayerBackCenter})
end