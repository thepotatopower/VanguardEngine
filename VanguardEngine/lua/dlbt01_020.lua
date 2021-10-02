-- Spirit Recharge! Luisa

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.Boosting, q.Other, o.This, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.FL, FL.PlayerBackCenter, q.Other, o.This, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, p.HasPrompt, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.Exists(1) and obj.PlayerHasState(ps.VanguardHasSungSongThisTurn) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n) 
	if n == 1 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.AddBattleOnlyPower(1, 5000)
		if obj.Exists(2) then
			obj.CounterCharge(1)
		end
	end
	return 0
end