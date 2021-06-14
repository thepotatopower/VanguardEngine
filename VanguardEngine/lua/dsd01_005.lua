-- Fire Slash Dragon, Inferno Sword

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end


function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, true, true
	elseif n == 2 then
		return a.OnBattleEnds, true, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsAttackingUnit() and not obj.IsVanguard() then
			return true
		end
	elseif n == 2 then
		if obj.IsAttackingUnit() and not obj.IsVanguard() then
			return true
		end
	end
	return false
end

function Cost(n)
end

function Activate(n)
	if n == 1 then
		obj.AddPower(2, 2000)
	elseif n == 2 then
		obj.AddPower(2, -2000)
	end
	return 0
end