-- Heartfelt Song, Loronerol

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerOrder, q.Other, o.Song, q.Other, o.FaceUp, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerOrder, q.Other, o.Song, q.Other, o.FaceDown, q.Count, 2
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, p.HasPrompt, p.OncePerTurn, p.CB, 1
	elseif n == 2 then
		return a.OnAttack, p.HasPrompt, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() then
			return true
		end
	elseif n == 2 then
		if obj.IsVanguard() and obj.IsAttackingUnit() and obj.Exists(2) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n) 
	if n == 1 then
		if obj.Exists(1) then
			return true
		end
	elseif n == 2 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.Sing(1)
	elseif n == 2 then
		obj.Sing(1)
		obj.AddUntilEndOfBattleEnemyValue(ps.CannotGuardWithUnitType, u.Sentinel)
	return 0
end