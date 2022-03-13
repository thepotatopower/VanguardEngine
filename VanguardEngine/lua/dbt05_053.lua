-- 3000年後の世界で ロロワ

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnVC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
	-- glitter
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetProperty(p.IsGlitter)
end

function Trigger()
	return obj.IsApplicable()
end

function Cost(check)
	if check then return obj.CanRetire({q.Location, l.PlayerRC, q.Count, 1}) end
	obj.ChooseRetire({q.Location, l.PlayerRC, q.Count, 1})
end

function Activation()
	obj.SuperiorCall({}, {}, 2, 1, "dbt05_t02", {}, {})
end