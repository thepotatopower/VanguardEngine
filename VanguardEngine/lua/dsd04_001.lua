-- 樹角獣王 マグノリア

function RegisterAbilities()
	-- on end of battle
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBattleEnds)
	ability1.SetLocation(l.VC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit()
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function Activation()
	if obj.PersonaRode() then
		obj.Select({q.Location, l.PlayerRC, q.Count, 3})
	else
		obj.Select({q.Location, l.PlayerRC, q.Count, 1})
	end
	obj.AddCardState({q.Location, l.Selected}, cs.CanAttackFromBackRow, p.UntilEndOfTurn)
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end