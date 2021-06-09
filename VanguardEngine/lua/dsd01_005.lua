-- Fire Slash Dragon, Inferno Sword

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.This
	end
end


function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, l.PlayerRC, true, true
	elseif n == 2 then
		return a.OnBattleEnds, l.PlayerRC, true, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsAttackingUnit() then
			return true
		end
	elseif n == 2 then
		if obj.IsAttackingUnit() then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.AddPower(2, 2000)
	elseif n == 2 then
		obj.AddPower(2, -2000)
	end
	return 0
end